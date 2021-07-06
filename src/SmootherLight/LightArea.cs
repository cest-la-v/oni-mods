using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AsLimc.SmootherLight
{
    public abstract class LightArea
    {
        private static readonly Vector2 _LD = new Vector2(-0.5f, -0.5f);
        private static readonly Vector2 _RD = new Vector2(0.5f, -0.5f);
        private static readonly Vector2 _LU = new Vector2(-0.5f, 0.5f);
        private static readonly Vector2 _RU = new Vector2(0.5f, 0.5f);
        private static readonly LineBlock _DOWN_RL = new LineBlock(_RD, _LD);
        private static readonly LineBlock _UP_LR = new LineBlock(_LU, _RU);
        private static readonly LineBlock _LEFT_DU = new LineBlock(_LD, _LU);
        private static readonly LineBlock _RIGHT_UD = new LineBlock(_RU, _RD);
        protected readonly float RangeSqr;
        private readonly List<LineBlock> _lineBlocks;
        private readonly Dictionary<Vector2, int> _pointCounts;

        protected LightArea(int range)
        {
            var r = range + 0.5f;
            RangeSqr = r * r;
            _lineBlocks = new List<LineBlock>();
            _pointCounts = new Dictionary<Vector2, int>();
        }

        public static LightArea Create(int range, Vector2 direction, float angle = 360f)
        {
            if (direction == Vector2.zero) return new CircleLightArea(range);

            return new SectorLightArea(range, direction, angle);
        }

        public void AddCellBlock(Vector2I deltaPoint)
        {
            var dx = deltaPoint.x;
            var dy = deltaPoint.y;
            switch (dx)
            {
                case 0 when dy == 0:
                    break;
                case 0 when dy > 0:
                    AddLineBlock(_DOWN_RL + deltaPoint);
                    break;
                case 0 when dy < 0:
                    AddLineBlock(_UP_LR + deltaPoint);
                    break;
                default:
                {
                    if (dx > 0 && dy == 0)
                    {
                        AddLineBlock(_LEFT_DU + deltaPoint);
                    }
                    else if (dx > 0 && dy > 0)
                    {
                        AddLineBlock(_DOWN_RL + deltaPoint);
                        AddLineBlock(_LEFT_DU + deltaPoint);
                    }
                    else if (dx > 0 && dy < 0)
                    {
                        AddLineBlock(_LEFT_DU + deltaPoint);
                        AddLineBlock(_UP_LR + deltaPoint);
                    }
                    else if (dx < 0 && dy == 0)
                    {
                        AddLineBlock(_RIGHT_UD + deltaPoint);
                    }
                    else if (dx < 0 && dy > 0)
                    {
                        AddLineBlock(_RIGHT_UD + deltaPoint);
                        AddLineBlock(_DOWN_RL + deltaPoint);
                    }
                    else if (dx < 0 && dy < 0)
                    {
                        AddLineBlock(_UP_LR + deltaPoint);
                        AddLineBlock(_RIGHT_UD + deltaPoint);
                    }

                    break;
                }
            }
        }

        public void AddLineBlock(LineBlock lineBlock)
        {
            var newLineBlock = lineBlock;
            for (var i = 0; i < _lineBlocks.Count; i++)
            {
                var block = _lineBlocks[i];
                if (!block.JoinIfAble(newLineBlock, out var joined)) continue;
                _lineBlocks.RemoveAt(i);
                i--;
                MinusPointCount(newLineBlock.Start);
                MinusPointCount(newLineBlock.End);
                newLineBlock = joined;
            }

            _lineBlocks.Add(newLineBlock);
            PlusPointCount(newLineBlock.Start);
            PlusPointCount(newLineBlock.End);
        }

        private void PlusPointCount(Vector2 point)
        {
            if (_pointCounts.ContainsKey(point))
            {
                _pointCounts[point]++;
            }
            else
            {
                _pointCounts[point] = 1;
            }
        }

        private void MinusPointCount(Vector2 point)
        {
            if (_pointCounts.ContainsKey(point) && _pointCounts[point] > 1)
            {
                _pointCounts[point]--;
            }
            else
            {
                _pointCounts.Remove(point);
            }
        }

        private bool DoesBlockPoint(Vector2 point)
        {
            return (_pointCounts.ContainsKey(point) ? _pointCounts[point] : 0) >= 2;
        }

        public abstract bool InRange(Vector2I deltaPoint);

        public bool IsBlocking(Vector2I deltaPoint)
        {
            return _lineBlocks.Any(block => block.IsBlocking(deltaPoint, DoesBlockPoint(block.Start), DoesBlockPoint(block.End)));
        }
    }

    public class CircleLightArea : LightArea
    {
        public CircleLightArea(int range) : base(range)
        {
        }

        public override bool InRange(Vector2I deltaPoint)
        {
            return deltaPoint.magnitudeSqr <= RangeSqr;
        }
    }

    internal class SectorLightArea : LightArea
    {
        private readonly float _angle;
        private readonly Vector2 _direction;

        public SectorLightArea(int range, Vector2 direction, float angle) : base(range)
        {
            _direction = direction;
            _angle = angle;
        }

        public override bool InRange(Vector2I deltaPoint)
        {
            return deltaPoint.magnitudeSqr <= RangeSqr
                   && Vector2.Angle(_direction, deltaPoint) <= _angle / 2;
        }
    }
}