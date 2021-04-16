using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject
{
    public interface IMainWorkSpace
    {
        string NameWorkSpace { get; }
        PackIcon IconDisplay { get; }
    }
}
