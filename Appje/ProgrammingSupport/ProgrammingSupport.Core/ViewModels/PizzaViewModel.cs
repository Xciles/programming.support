using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xciles.Uncommon.Net;

namespace ProgrammingSupport.Core.ViewModels
{
    public class PizzaViewModel : MvxViewModel
    {
        private const string PizzaEndpoint = "https://hackathon-menu.dominos.cloud/Rest/nl/menus/30544/en";
        private IList<Product> _pizzas;

        public IList<Product> Pizzas
        {
            get
            {
                return _pizzas;
            }
            set { _pizzas = value; }
        }

        public PizzaViewModel()
        {
            GetPizzaaass().ConfigureAwait(false);
        }

        public async Task GetPizzaaass()
        {
            var result = await UncommonRequestHelper.ProcessGetRequestAsync<Menu>(PizzaEndpoint).ConfigureAwait(false);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                Pizzas = result.Result.MenuPages.SelectMany(x => x.SubMenus.SelectMany(y => y.Products.ToList())).ToList();
            }
        }
    }
}
