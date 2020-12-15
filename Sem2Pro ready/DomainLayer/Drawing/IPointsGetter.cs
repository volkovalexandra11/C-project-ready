﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZedGraph;

namespace FunctionGraph
{
    public interface IPointsGetter
    {
        PointPairList GetPointsForOneFunction(Function function, double leftBound, double rightBound);
    }
}