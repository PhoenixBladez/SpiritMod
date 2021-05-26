using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.StarjinxSet
{
	public class ManajinxBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Starlight Enchantment");
			Description.SetDefault("Magic weapons spawn additional homing stars, and mana usage is drastically decreased");
			Main.buffNoSave[Type] = true;
		}
		
		public override void Update(Player player, ref int buffIndex)
		{
			player.manaCost /= 2;
		}
	}
}