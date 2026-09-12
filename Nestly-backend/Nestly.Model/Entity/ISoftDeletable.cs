using System;

namespace Nestly.Model.Entity
{
    // Marks an entity as soft-deletable: "deleting" it sets these two
    // properties instead of removing the row, and NestlyDbContext applies a
    // global query filter (WHERE IsDeleted = 0) to every entity that
    // implements this interface, so a soft-deleted row simply stops
    // appearing anywhere without every Get/List query needing to know
    // about it.
    //
    // Scoped to leaf-ish daily-record entities that don't cascade-delete
    // children (DiaperLog, FeedingLog, HealthEntry, SleepLog, SymptomDiary,
    // Milestone, MealPlan, CalendarEvent, Pregnancy, QaQuestion) - entities
    // with owned child rows (BabyProfile, BlogPost, BlogCategory,
    // MedicationPlan, AppUser) are intentionally left on hard delete for
    // now, since soft-deleting a parent without also soft-deleting/hiding
    // its children would leave those children visible but orphaned. That
    // remains a documented follow-up rather than something silently
    // half-done here.
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
        DateTime? DeletedAt { get; set; }
    }
}
