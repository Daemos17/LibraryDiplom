//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace datamodel
{
    using System;
    using System.Collections.Generic;
    
    public partial class vAccount
    {
        public string Login { get; set; }
        public Nullable<System.DateTime> RegistrationDate { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public int Id { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public string Email { get; set; }
    }
}
