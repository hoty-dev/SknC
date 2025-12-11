/*
 * =========================================================================================
 * Copyright (c) 2025 Javier Granero. All rights reserved.
 * * Project: SknC (Skincare Management System)
 * Author: Javier Granero
 * Date: 10/12/2025
 * * This software is the confidential and proprietary information of the author.
 * =========================================================================================
*/

using Microsoft.EntityFrameworkCore;
using SknC.Web.Infrastructure.Data;

namespace SknC.Web.Services
{
    public interface IStatisticsService
    {
        Task<int> CalculateCurrentStreakAsync(string userId);
        Task<int> CalculateMonthlyConsistencyAsync(string userId);
    }

    public class StatisticsService : IStatisticsService
    {
        private readonly AppDbContext _context;

        public StatisticsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> CalculateCurrentStreakAsync(string userId)
        {
            // 1. Get all unique dates where the user executed a routine
            var executionDates = await _context.RoutineExecutions
                .Where(e => e.Routine != null && e.Routine.UserId == userId)
                .Select(e => e.DateExecuted.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToListAsync();

            if (!executionDates.Any()) return 0;

            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);

            // 2. Check if the streak is alive (must have done something today or yesterday)
            // If the last execution was before yesterday, the streak is broken (0).
            if (executionDates.First() < yesterday) return 0;

            // 3. Count consecutive days
            int streak = 0;
            var checkDate = executionDates.First() == today ? today : yesterday;

            foreach (var date in executionDates)
            {
                if (date == checkDate)
                {
                    streak++;
                    checkDate = checkDate.AddDays(-1);
                }
                else
                {
                    // Gap found, stop counting
                    break;
                }
            }

            return streak;
        }

        public async Task<int> CalculateMonthlyConsistencyAsync(string userId)
        {
            var today = DateTime.Today;
            var startOfMonth = new DateTime(today.Year, today.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(today.Year, today.Month);

            // Count distinct days with activity in current month
            var activeDays = await _context.RoutineExecutions
                .Where(e => e.Routine != null && 
                            e.Routine.UserId == userId && 
                            e.DateExecuted >= startOfMonth && 
                            e.DateExecuted <= today) // Up to today? Or end of month? Usually up to now.
                .Select(e => e.DateExecuted.Date)
                .Distinct()
                .CountAsync();

            // Calculate percentage
            if (daysInMonth == 0) return 0;
            return (int)((double)activeDays / daysInMonth * 100);
        }
    }
}