using UnityEngine;

namespace AsLimc.SmootherLight
{
    public struct LineBlock
    {
        public Vector2 Start { get; }
        public Vector2 End { get; }
        private readonly float _angle; // (0, 180)
        private readonly Vector2 _direction;

        public LineBlock(Vector2 start, Vector2 end)
        {
            Start = start;
            End = end;
            _angle = Vector2.SignedAngle(Start, End);
            _direction = End - Start;
        }

        public bool IsBlocking(Vector2 deltaPoint, bool blockStart = true, bool blockEnd = true)
        {
            var deltaPointAngle = Vector2.SignedAngle(Start, deltaPoint);
            return (blockStart ? deltaPointAngle >= 0f : deltaPointAngle > 0f)
                   && (blockEnd ? deltaPointAngle <= _angle : deltaPointAngle < _angle)
                   && Vector2.SignedAngle(_direction, deltaPoint - Start) <= 0f;
        }

        public bool JoinIfAble(LineBlock other, out LineBlock joined)
        {
            // if appendable
            var sameDirection = Vector2.Angle(_direction, other._direction) <= 0;
            if (sameDirection)
            {
                if (Start == other.End)
                {
                    joined = new LineBlock(other.Start, End);
                    return true;
                }

                if (End == other.Start)
                {
                    joined = new LineBlock(Start, other.End);
                    return true;
                }
            }

            // if totally blocked
            var isThisShorter = _angle < other._angle;
            var longer = isThisShorter ? other : this;
            var shorter = isThisShorter ? this : other;
            if (longer.IsBlocking(shorter.Start) && longer.IsBlocking(shorter.End))
            {
                joined = longer;
                return true;
            }

            // not joined, useless
            joined = this;
            return false;
        }

        public static LineBlock operator +(LineBlock lineBlock, Vector2 offset)
        {
            return new LineBlock(lineBlock.Start + offset, lineBlock.End + offset);
        }
    }
}