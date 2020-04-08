using System;
using System.Collections.Generic;
using System.Text;

namespace Core.ViewModel
{
    public class GetTaskVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; }
        public byte Hours { get; set; }
        public byte Minutes { get; set; }
    }
}
