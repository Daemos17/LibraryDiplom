
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
    
public partial class Teacher
{

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
    public Teacher()
    {

        this.ReaderBooks = new HashSet<ReaderBook>();

    }


    public int Id { get; set; }

    public Nullable<int> User_id { get; set; }

    public string Comment { get; set; }



    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

    public virtual ICollection<ReaderBook> ReaderBooks { get; set; }

}

}
