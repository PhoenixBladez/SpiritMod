using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
	public class PhantomVeil : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Phantom Veil");
			Description.SetDefault("The next attack will be blocked");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			player.lifeRegen += 8;
			player.buffTime[buffIndex] = modPlayer.glyph == GlyphType.Veil ? 2 : 0;
		}
	}
}
