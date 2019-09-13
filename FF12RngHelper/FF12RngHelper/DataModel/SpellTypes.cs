using System.ComponentModel;

namespace FF12RngHelper.DataModel
{
    /// <summary>
    /// Enumeration of all supported spells
    /// </summary>
    public enum SpellTypes
    {
        [Description("Cure")]
        Cure = 20,

        [Description("Cura")]
        Cura = 45,

        [Description("Curaga")]
        Curaga = 85,

        [Description("Curaja")]
        Curaja = 145,

        [Description("Cura IZJS/TZA")]
        CuraIzjsTza = 46,

        [Description("Curaga IZJS/TZA")]
        CuragaIzjsTza = 86,

        [Description("Curaja IZJS/TZA")]
        CurajaIzjsTza = 128
    }
}