using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using SpiritMod.Mechanics.BoonSystem;
using SpiritMod.Utilities;
using SpiritMod.Buffs;
using Terraria.Audio;

namespace SpiritMod.NPCs.Hydra
{
	public class Hydra : ModNPC
	{
		private const int MAXHEADS = 4;

		private bool initialized = false;

		private List<NPC> heads = new List<NPC>();

		public int newHeadCountdown = -1;
		public int headsDue = 1;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
		}
		public override void SetDefaults()
		{
			npc.width = 36;
			npc.height = 36;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 180f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
			npc.immortal = true;
			npc.dontTakeDamage = true;
			npc.hide = true;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int x = spawnInfo.spawnTileX;
			int y = spawnInfo.spawnTileY;
			int tile = (int)Main.tile[x, y].type;
			return (tile == 367) && spawnInfo.spawnTileY > Main.rockLayer && Main.hardMode ? 0.5f : 0f;
		}

		public override void AI()
		{
			npc.TargetClosest(true);
			if (!initialized)
			{
				initialized = true;
				for (int i = 0; i < 2; i++)
				{
					SpawnHead(npc.lifeMax);
				}
			}

			foreach (NPC head in heads.ToArray())
			{
				if (!head.active)
					heads.Remove(head);
			}

			if (heads.Count <= 0)
			{
				npc.life = 0;
				npc.StrikeNPC(1, 0, 0);
			}

			newHeadCountdown--;
			if (newHeadCountdown == 0)
			{
				for (int i = 0; i < headsDue; i++)
				{
					SpawnHead(npc.lifeMax);
				}
				headsDue = 1;
			}
		}

		public void SpawnHead(int life)
		{
			if (heads.Count >= MAXHEADS)
				return;
			int npcIndex = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<HydraHead>(), 0, npc.whoAmI);
			NPC head = Main.npc[npcIndex];
			head.life = head.lifeMax = life;
			heads.Add(head);
		}
	}

	public enum HeadColor
	{
		Red = 0,
		Green = 1,
		Purple = 2
	}
	public class HydraHead : ModNPC
	{
		private bool initialized = false;

		private NPC parent => Main.npc[(int)npc.ai[0]];

		private HeadColor headColor;

		private Vector2 posToBe = Vector2.Zero;
		private float rotation;
		private float sway;

		private float centralRotation;
		private int centralDistance;
		private float rotationSpeed;
		private float swaySpeed;
		private Vector2 orbitRange;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lernean Hydra");
		}
		public override void SetDefaults()
		{
			npc.width = 44;
			npc.height = 32;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath14;
			npc.value = 180f;
			npc.knockBackResist = 0;
			npc.noGravity = true;
			npc.noTileCollide = true;
		}

		public override void AI()
		{
			if (!initialized)
			{
				initialized = true;
				headColor = (HeadColor)Main.rand.Next(3);

				rotation = Main.rand.NextFloat(6.28f);
				sway = Main.rand.NextFloat(6.28f);

				centralDistance = Main.rand.Next(150, 200);
				rotationSpeed = Main.rand.NextFloat(0.03f, 0.05f);
				orbitRange = Main.rand.NextVector2Circular(100, 50);
				swaySpeed = Main.rand.NextFloat(0.015f, 0.035f);
			}
			if (!parent.active)
			{
				npc.active = false;
				return;
			}

			rotation += rotationSpeed;
			sway += swaySpeed;

			npc.spriteDirection = npc.direction = parent.direction;
			centralRotation = 0.3f * ((float)Math.Sin(sway) + (npc.direction * 1.5f));
			posToBe = DecidePosition();

			MoveToPosition();

			if (npc.direction == 1)
			{
				npc.rotation = npc.DirectionTo(Main.player[parent.target].Center).ToRotation();
			}
			else
			{
				npc.rotation = npc.DirectionTo(Main.player[parent.target].Center).ToRotation() - 3.14f;
			}

			if (!parent.active)
				npc.active = false;
		}

		private Vector2 DecidePosition()
		{
			Vector2 pos = new Vector2(0, -1).RotatedBy(centralRotation) * centralDistance;
			pos += orbitRange.RotatedBy(rotation);
			pos += parent.Center;
			return pos;
		}

		private void MoveToPosition()
		{
			npc.Center = Vector2.Lerp(npc.Center, posToBe, 0.05f);
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0 && parent.modNPC is Hydra modNPC)
			{
				if (modNPC.newHeadCountdown < 0)
					modNPC.newHeadCountdown = 150;
				modNPC.headsDue++;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			string colorString = getColor();
			string texturePath = Texture + colorString;
			Texture2D headTex = ModContent.GetTexture(texturePath);
			Texture2D neckTex = ModContent.GetTexture(texturePath + "_Neck");

			Vector2 direction = npc.Center - parent.Bottom;
			Vector2 centralPos = (new Vector2(0, -1) * direction.Length());

			Vector2 cPointA = parent.Bottom + (centralPos.RotatedBy(centralRotation) * 0.33f);
			Vector2 cPointB = parent.Bottom + (centralPos.RotatedBy(centralRotation / 2) * 0.66f);

			BezierCurve curve = new BezierCurve(new Vector2[] { parent.Bottom, cPointA, cPointB, npc.Center });

			int numPoints = 30; //Should make dynamic based on curve length, but I'm not sure how to smoothly do that while using a bezier curve
			Vector2[] chainPositions = curve.GetPoints(numPoints).ToArray();

			//Draw each chain segment, skipping the very first one, as it draws partially behind the player
			for (int i = 1; i < numPoints; i++)
			{
				Vector2 position = chainPositions[i];

				float rotation = (chainPositions[i] - chainPositions[i - 1]).ToRotation() - MathHelper.PiOver2; //Calculate rotation based on direction from last point
				float yScale = Vector2.Distance(chainPositions[i], chainPositions[i - 1]) / neckTex.Height; //Calculate how much to squash/stretch for smooth chain based on distance between points

				Vector2 scale = new Vector2(1, yScale); // Stretch/Squash chain segment
				Color chainLightColor = Lighting.GetColor((int)position.X / 16, (int)position.Y / 16); //Lighting of the position of the chain segment
				Vector2 origin = new Vector2(neckTex.Width / 2, neckTex.Height); //Draw from center bottom of texture
				spriteBatch.Draw(neckTex, position - Main.screenPosition, null, chainLightColor, rotation, origin, scale, npc.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			}

			spriteBatch.Draw(headTex, npc.Center  - Main.screenPosition, null, drawColor, npc.rotation, new Vector2(headTex.Width, headTex.Height) / 2, npc.scale, npc.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

		private string getColor()
		{
			switch ((int)headColor)
			{
				case 0:
					return "_Red";
				case 1:
					return "_Green";
				case 2:
					return "_Purple";
				default:
					return "_Red";
			}
		}
	}
}