using System;
using System.Collections.Generic;

namespace Lesson5_webapi.Models;

public partial class Score
{
    public string StudentId { get; set; } = null!;

    public string CourseId { get; set; } = null!;

    public decimal Score1 { get; set; }

    public virtual Course Course { get; set; } = null!;

    public virtual Student Student { get; set; } = null!;
}
