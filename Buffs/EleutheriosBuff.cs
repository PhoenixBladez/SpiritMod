using SpiritMod.Items.Sets.OlympiumSet;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class EleutheriosBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Eleutherios BUff");
			Description.SetDefault("Damage increased by x%");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = false;
		}

		public override void ModifyBuffTip(ref string tip, ref int rare) => //MIGHT be bad for MP but I don't know how to get player otherwise
			tip = $"Damage increased by {System.Math.Truncate(Main.LocalPlayer.GetModPlayer<OlympiumPlayer>().eleutheoriosStrength * 100)}%";

		public override void Update(Player player, ref int buffIndex)
		{
			int time = player.buffTime[buffIndex];
			player.GetModPlayer<OlympiumPlayer>().eleutheoriosStrength = time * 0.025f / 60f;
		}
	}
}
