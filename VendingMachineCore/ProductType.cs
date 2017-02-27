//---------------------------------------------------------------------------//
//Разработчик: Гаппов Борис, gappov@gmail.com, +79262302992                  //
//Москва, 26.09.2014                                                         //
//---------------------------------------------------------------------------//
namespace VendingMachineCore
{
    /// <summary>
    /// Тип продукта
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Название продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Стоимость за штуку
        /// </summary>
        public int Price { get; set; }
    }
}
