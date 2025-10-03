using Microsoft.EntityFrameworkCore;
using Puroguramu.Domains.Models;
using Puroguramu.Domains.Repositories;
using Puroguramu.Infrastructures.DbContexts;

namespace Puroguramu.Infrastructures.Repositories;

public class UpdateLessonRepository : IUpdateLessonRepository
{
    private PuroguramuDbContext _context;

    public UpdateLessonRepository(PuroguramuDbContext context)
    {
        _context = context;
    }

    public async Task MoveLessonUpAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        var previousLesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Position == lesson.Position - 1);
        if (previousLesson != null)
        {
            previousLesson.Position += 1;
            lesson.Position -= 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task MoveLessonDownAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        var nextLesson = await _context.Lessons.FirstOrDefaultAsync(l => l.Position == lesson.Position + 1);
        if (nextLesson != null)
        {
            nextLesson.Position -= 1;
            lesson.Position += 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task HideLessonAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        lesson.IDStatut = 1;
        await _context.SaveChangesAsync();
    }

    public async Task UnHideLessonAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        lesson.IDStatut = 2;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteLessonAsync(int lessonId)
    {
        var lesson = await _context.Lessons.FindAsync(lessonId);
        var followingLessons = await _context.Lessons.Where(l => l.Position > lesson.Position).ToListAsync();
        foreach (var followingLesson in followingLessons)
        {
            followingLesson.Position -= 1;
        }

        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateLessonAsync(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CreateLessonAsync(string title)
    {
        var lessons = _context.Lessons;
        var position = await lessons.AnyAsync() ? await lessons.MaxAsync(l => l.Position) + 1 : 1;

        var lesson = new Lesson { Intitule = title, Position = position, IDStatut = 1 };

        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return lesson.IDLecon;
    }
}
