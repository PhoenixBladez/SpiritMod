using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Accessory.SanguineWardTree
{
	public class RunicSiphon : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Runic Siphon");
			Description.SetDefault("Reduced damage resistance and losing HP rapidly");
			Main.buffNoTimeDisplay[Type] = false;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.lifeRegen > 0) 
				npc.lifeRegen = 0;

			npc.lifeRegen -= 8;
		}
	}

	public class RunicSiphonGNPC : GlobalNPC
	{
		private float _runeGlow;
		public override bool CloneNewInstances => true;
		public override bool InstancePerEntity => true;
		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (npc.HasBuff(ModContent.BuffType<RunicSiphon>()))
				damage = (int)(damage * 1.15);
		}

		public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (npc.HasBuff(ModContent.BuffType<RunicSiphon>()))
				damage = (int)(damage * 1.15);
		}

		public override void PostAI(NPC npc) => _runeGlow = (npc.HasBuff(ModContent.BuffType<RunicSiphon>())) ? Math.Min(_runeGlow + 0.05f, 0.5f) : Math.Max(_runeGlow - 0.05f, 0);

		public override Color? GetAlpha(NPC npc, Color drawColor) => Color.Lerp(drawColor, new Color(250, 85, 167), _runeGlow) * npc.Opacity;
	}
}
