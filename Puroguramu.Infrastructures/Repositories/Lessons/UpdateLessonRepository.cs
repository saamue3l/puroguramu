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

    public void MoveLessonUp(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);
        var previousLesson = _context.Lessons.FirstOrDefault(l => l.Position == lesson.Position - 1);
        if (previousLesson != null)
        {
            previousLesson.Position += 1;
            lesson.Position -= 1;
            _context.SaveChanges();
        }
    }

    public void MoveLessonDown(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);
        var nextLesson = _context.Lessons.FirstOrDefault(l => l.Position == lesson.Position + 1);
        if (nextLesson != null)
        {
            nextLesson.Position -= 1;
            lesson.Position += 1;
            _context.SaveChanges();
        }
    }

    public void HideLesson(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);
        lesson.IDStatut = 1;
        _context.SaveChanges();
    }

    public void UnHideLesson(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);
        lesson.IDStatut = 2;
        _context.SaveChanges();
    }

    public void DeleteLesson(int lessonId)
    {
        var lesson = _context.Lessons.Find(lessonId);
        var followingLessons = _context.Lessons.Where(l => l.Position > lesson.Position);
        foreach (var followingLesson in followingLessons)
        {
            followingLesson.Position -= 1;
        }

        _context.Lessons.Remove(lesson);
        _context.SaveChanges();
    }

    public void UpdateLesson(Lesson lesson)
    {
        _context.Lessons.Update(lesson);
        _context.SaveChanges();
    }

    public int CreateLesson(string title)
    {
        var lessons = _context.Lessons;
        var position = lessons.Any() ? lessons.Max(l => l.Position) + 1 : 1;

        var lesson = new Lesson { Intitule = title, Position = position, IDStatut = 1 };

        _context.Lessons.Add(lesson);
        _context.SaveChanges();
        return lesson.IDLecon;
    }
}
