using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainProject.StatisticWorkSpace
{
    class DetailStatisticViewModel : StatisticViewModel
    {
        public new String CreateTitle(StatisticModel model)
        {
            return model.Title;
        }

        public DetailStatisticViewModel()
        {
            listModel = new List<StatisticModel>
            {
                new StatisticModel(){ Title = "Title1", Revenue = 10000},
                new StatisticModel(){ Title = "Title2", Revenue = 20000},
                new StatisticModel(){ Title = "Title3", Revenue = 15000},
                new StatisticModel(){ Title = "Title4", Revenue = 100000},
                new StatisticModel(){ Title = "Title5", Revenue = 130000}
            };
            foreach (var model in ListModel) { model.Label = CreateLabel(model); model.Title = CreateTitle(model); }
            SelectedOptionProduct = OPTION_ALL_PRODUCT;
            formaterLabelAxisY = val => getMoneyLabel((int)val);
            this.PropertyChanged += StatisticViewModel_PropertyChanged;
        }
    }
}
