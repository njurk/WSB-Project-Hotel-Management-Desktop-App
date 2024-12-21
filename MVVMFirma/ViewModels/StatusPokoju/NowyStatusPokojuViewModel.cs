﻿using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NowyStatusPokojuViewModel : JedenViewModel<StatusPokoju>
    {
        #region Constructor
        public NowyStatusPokojuViewModel()
            : base("Status pokoju")
        {
            item = new StatusPokoju();
        }
        #endregion

        #region Properties
        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            hotelEntities.StatusPokoju.Add(item);
            hotelEntities.SaveChanges();
        }
        #endregion
    }
}
