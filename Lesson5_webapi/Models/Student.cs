using System;
using System.Collections.Generic;

namespace Lesson5_webapi.Models;

public partial class Student
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public DateOnly BirthDate { get; set; }

    public string Gender { get; set; } = null!;

    public virtual ICollection<Score> Scores { get; set; } = new List<Score>();
}
