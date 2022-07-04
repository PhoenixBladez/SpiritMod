using Microsoft.Xna.Framework;
using SpiritMod.Items.Weapon.Thrown;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.OceanSlime
{
	public class OceanSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coconut Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			NPC.width = 18;
			NPC.height = 14;
			NPC.damage = 17;
			NPC.defense = 6;
			NPC.lifeMax = 60;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.value = 860f;
			NPC.knockBackResist = .45f;
			NPC.aiStyle = 1;
			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.CoconutSlimeBanner>();
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot) => npcLoot.AddCommon<Coconut>(6, 9);

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 30; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.DynastyWood, 2.5f * hitDirection, -2.5f, 0, default, .34f);
			}

			if (NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("OceanSlime1").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("OceanSlime2").Type, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("OceanSlime3").Type, 1f);
			}
		}
	}
}