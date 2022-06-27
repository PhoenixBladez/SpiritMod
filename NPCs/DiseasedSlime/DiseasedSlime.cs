using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.BismiteSet;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using SpiritMod.Buffs.DoT;
using Terraria.ModLoader.Utilities;

namespace SpiritMod.NPCs.DiseasedSlime
{
	public class DiseasedSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxious Slime");
			Main.npcFrameCount[NPC.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			NPC.width = 44;
			NPC.height = 32;
			NPC.damage = 18;
			NPC.defense = 5;
			NPC.lifeMax = 65;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath22;
			NPC.buffImmune[BuffID.Poisoned] = true;
			NPC.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			NPC.buffImmune[BuffID.Venom] = true;
			NPC.value = 42f;
			NPC.alpha = 45;
			NPC.knockBackResist = .6f;
			NPC.aiStyle = 1;
			AIType = NPCID.BlueSlime;
			AnimationType = NPCID.BlueSlime;
			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.DiseasedSlimeBanner>();
		}

		public bool hasPicked = false;
		int pickedType;

		public override void AI()
		{
			if (!hasPicked)
			{
				NPC.scale = Main.rand.NextFloat(.9f, 1f);
				pickedType = Main.rand.Next(0, 5);
				hasPicked = true;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frame.X = 50 * pickedType;
			NPC.frame.Width = 50;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(pickedType);
			writer.Write(hasPicked);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			pickedType = reader.ReadInt32();
			hasPicked = reader.ReadBoolean();
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
				return 0f;
			return SpawnCondition.Underground.Chance * 0.27f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Vector2 extraOffset = new Vector2(-26, -17);
			Vector2 drawOrigin = new Vector2(TextureAssets.Npc[NPC.type].Value.Width * 0.5f, (NPC.height * 0.5f));
			Vector2 drawPos = NPC.Center - Main.screenPosition + drawOrigin + extraOffset;
			Color color = NPC.GetAlpha(lightColor);
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos, new Microsoft.Xna.Framework.Rectangle?(NPC.frame), color, NPC.rotation, drawOrigin, NPC.scale, effects, 0f);
			return false;
		}

		public override void OnKill()
		{
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<BismiteCrystal>(), Main.rand.Next(4, 7));
			Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.Gel, Main.rand.Next(1, 3) + 1);

			if (Main.rand.Next(10000) == 0)
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ItemID.SlimeStaff);

			if (Main.rand.NextBool(25))
				Item.NewItem((int)NPC.position.X, (int)NPC.position.Y, NPC.width, NPC.height, ModContent.ItemType<Cake>());
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 240);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life > 0)
			{
				for (int k = 0; k < 12; k++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, hitDirection, -2.5f, 0, Color.Green * .14f, 0.7f);
			}
			else
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousGas>(), 0, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousIndicator>(), 0, 1, Main.myPlayer, 0, 0);
					SoundEngine.PlaySound(SoundLoader.customSoundType, NPC.position, Mod.GetSoundSlot(SoundType.Custom, "Sounds/GasHiss"));
				}
				for (int k = 0; k < 25; k++)
					Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.SlimeBunny, 2 * hitDirection, -2.5f, 0, Color.Green * .14f, 0.7f);
			}
		}
	}
}