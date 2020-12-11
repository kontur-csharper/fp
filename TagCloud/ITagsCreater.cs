﻿using System;
using System.Collections.Generic;
using System.Drawing;
using ResultOf;

namespace TagCloud
{
    public interface ITagsCreater
    {
        Result<List<Tuple<string, Rectangle>>> GetTags(string filename, int canvasHeight);
    }
}