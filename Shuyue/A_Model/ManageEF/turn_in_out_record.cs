//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace ManageEF
{
    using System;
    using System.Collections.Generic;
    
    public partial class turn_in_out_record
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public decimal Money { get; set; }
        public System.DateTime OperationDate { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
    }
}
