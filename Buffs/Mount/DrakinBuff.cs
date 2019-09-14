using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
    public class DrakinBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Drakin Mount");
			Description.SetDefault("A wild ride indeed");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
            player.GetSpiritPlayer().drakinMount = true;
            player.mount.SetMount(mod.MountType("DrakinMount"), player, false);
			player.buffTime[buffIndex] = 10;
			player.allDamage += 0.05f;
			player.statDefense += 10;
		}
	}
}
