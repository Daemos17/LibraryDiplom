
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
    
public partial class ReaderBook
{

    public int IdRead { get; set; }

    public Nullable<int> BookId { get; set; }

    public Nullable<int> ReaderId { get; set; }

    public Nullable<System.DateTime> DateOfTaking { get; set; }

    public Nullable<System.DateTime> DateOfReturning { get; set; }

    public Nullable<int> CounOfBooks { get; set; }



    public virtual Book Book { get; set; }

    public virtual Student Student { get; set; }

}

}
