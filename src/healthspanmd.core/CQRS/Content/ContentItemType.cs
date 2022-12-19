using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace healthspanmd.core.CQRS.Content
{
    public enum ContentItemType
    {
        Video_YouTube = 0,
        Image = 1,
        PDFDocument = 2,
        WebLink = 3,
        Text = 4,
        Video_Vimeo = 5
    }
}
