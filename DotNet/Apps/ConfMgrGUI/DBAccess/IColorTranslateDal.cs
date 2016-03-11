using OpsTrackingModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBAccess
{
    public interface IColorTranslateDal
    {
        List<ColorTranslate> GetAllStub();
        List<ColorTranslate> GetAll();
    }
}
