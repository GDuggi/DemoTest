﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IFaxLogSentDal
    {
        void Insert(FaxLogSentDto pData);
    }
}
