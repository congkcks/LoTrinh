using System;
using System.Collections.Generic;

namespace Service2.Models;

public partial class UserGrammarProgress
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int GrammarExerciseId { get; set; }

    public decimal? Score { get; set; }

    public bool? IsCompleted { get; set; }

    public DateTime? CompletedAt { get; set; }
}
