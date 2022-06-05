using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using datamodel;

namespace libbook.ViewModels
{
    public partial class BarCodeViewModel
    {

        public Book Book { get; set; }
        public byte[] QrCodeImage { get; set; }
    }
}