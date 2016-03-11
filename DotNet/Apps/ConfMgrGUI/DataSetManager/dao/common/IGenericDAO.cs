using System;
using System.Collections.Generic;
using System.Text;
//using GigaSpaces.Core;

namespace DataManager.common
{
    public interface IGenericDAO<T>
    {
        //IList<T> gsCreateObjList(T template, ISpaceProxy space);
        IList<T> dbCreateObjList(T template);
    }
}
