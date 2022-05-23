using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs;
using SpiritMod.Items.Sets.BismiteSet;
using SpiritMod.Items.Consumable.Food;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using SpiritMod.Buffs.DoT;

namespace SpiritMod.NPCs.DiseasedSlime
{
	public class DiseasedSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Noxious Slime");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
		}

		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 32;
			npc.damage = 18;
			npc.defense = 5;
			npc.lifeMax = 65;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath22;
			npc.buffImmune[BuffID.Poisoned] = true;
			npc.buffImmune[ModContent.BuffType<FesteringWounds>()] = true;
			npc.buffImmune[BuffID.Venom] = true;
			npc.value = 42f;
			npc.alpha = 45;
			npc.knockBackResist = .6f;
			npc.aiStyle = 1;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.DiseasedSlimeBanner>();
		}

		public bool hasPicked = false;
		int pickedType;

		public override void AI()
		{
			if (!hasPicked)
			{
				npc.scale = Main.rand.NextFloat(.9f, 1f);
				pickedType = Main.rand.Next(0, 5);
				hasPicked = true;
			}
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frame.X = 50 * pickedType;
			npc.frame.Width = 50;
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
			if (spawnInfo.playerSafe)
				return 0f;
			return SpawnCondition.Underground.Chance * 0.27f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 extraOffset = new Vector2(-26, -17);
			Vector2 drawOrigin = new Vector2(Main.npcTexture[npc.type].Width * 0.5f, (npc.height * 0.5f));
			Vector2 drawPos = npc.Center - Main.screenPosition + drawOrigin + extraOffset;
			Color color = npc.GetAlpha(lightColor);
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(Main.npcTexture[npc.type], drawPos, new Microsoft.Xna.Framework.Rectangle?(npc.frame), color, npc.rotation, drawOrigin, npc.scale, effects, 0f);
			return false;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<BismiteCrystal>(), Main.rand.Next(4, 7));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(1, 3) + 1);

			if (Main.rand.Next(10000) == 0)
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.SlimeStaff);

			if (Main.rand.NextBool(25))
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Cake>());
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(ModContent.BuffType<FesteringWounds>(), 240);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life > 0)
			{
				for (int k = 0; k < 12; k++)
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.SlimeBunny, hitDirection, -2.5f, 0, Color.Green * .14f, 0.7f);
			}
			else
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousGas>(), 0, 1, Main.myPlayer, 0, 0);
					Projectile.NewProjectile(npc.Center.X, npc.Center.Y, 0f, 0f, ModContent.ProjectileType<NoxiousIndicator>(), 0, 1, Main.myPlayer, 0, 0);
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/GasHiss"));
				}
				for (int k = 0; k < 25; k++)
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.SlimeBunny, 2 * hitDirection, -2.5f, 0, Color.Green * .14f, 0.7f);
			}
		}
	}
}