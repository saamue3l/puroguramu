using Puroguramu.Domains.Models;

namespace Puroguramu.Domains.Repositories;

public interface IUpdateLessonRepository
{
    public Task MoveLessonUpAsync(int lessonId);

    public Task MoveLessonDownAsync(int lessonId);

    public Task HideLessonAsync(int lessonId);

    public Task UnHideLessonAsync(int lessonId);

    public Task DeleteLessonAsync(int lessonId);

    public Task UpdateLessonAsync(Lesson lesson);

    public Task<int> CreateLessonAsync(string Title);

}
