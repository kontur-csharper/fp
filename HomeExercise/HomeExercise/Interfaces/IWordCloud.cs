﻿using System.Collections.Generic;
using System.Drawing;

namespace HomeExercise
{
    public interface IWordCloud
    {
        List<ISizedWord> SizedWords { get; }
        Point Center { get; }
        Size Size { get; }

        void BuildCloud();
    }
}