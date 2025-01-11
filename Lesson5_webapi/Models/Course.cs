using System;
using System.Collections.Generic;

namespace Lesson5_webapi.Models;

public partial class Course
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string TeacherId { get; set; } = null!;

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();

    public virtual Teacher Teacher { get; set; } = null!;
}
