﻿using System;
using System.Collections.Generic;

namespace Lesson5_webapi.Models;

public partial class Teacher
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
}
