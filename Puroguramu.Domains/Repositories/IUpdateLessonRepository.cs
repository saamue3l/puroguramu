using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IUpdateLessonRepository
{
    public void MoveLessonUp(int lessonId);

    public void MoveLessonDown(int lessonId);

    public void HideLesson(int lessonId);

    public void UnHideLesson(int lessonId);

    public void DeleteLesson(int lessonId);

    public void UpdateLesson(Lesson lesson);

    public int CreateLesson(string Title);

}
