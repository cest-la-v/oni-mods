using System.Collections;
using System.Collections.Generic;

namespace AsLimc.SmootherLight
{
    public interface ICellEnumerator : IEnumerable<Vector2I>
    {
    }

    public class RectBorder : ICellEnumerator
    {
        private readonly Vector2I _origin;
        private readonly int _range;

        public RectBorder(Vector2I origin, int range)
        {
            _origin = origin;
            _range = range;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vector2I> GetEnumerator()
        {
            yield return _origin;

            for (var depth = 1; depth <= _range; depth++)
            {
                var side = depth * 2 + 1;
                var minX = _origin.x - depth;
                var minY = _origin.y - depth;
                var maxX = minX - 1 + side;
                var maxY = minY - 1 + side;
                int x = minX, y = minY;
                do
                {
                    yield return new Vector2I(x, y);

                    // move
                    if (y == minY && x >= minX && x < maxX)
                        // bottom
                        x += 1;
                    else if (x == maxX && y >= minY && y < maxY)
                        // right
                        y += 1;
                    else if (y == maxY && x <= maxX && x > minX)
                        // top
                        x -= 1;
                    else
                        // left
                        y -= 1;
                } while (!(x == minX && y == minY));
            }
        }
    }

    public class TrapezoidLayer : ICellEnumerator
    {
        private readonly Vector2I _left = new Vector2I(-1, 0);
        private readonly Vector2I _leftDown = new Vector2I(-1, -1);
        private readonly Vector2I _origin;
        private readonly int _range;

        public TrapezoidLayer(Vector2I origin, int range)
        {
            _origin = origin;
            _range = range;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<Vector2I> GetEnumerator()
        {
            var start = _origin + _left;
            for (var depth = 0; depth <= _range; depth++, start += _leftDown)
            {
                var width = depth * 2 + 3;
                for (var i = 0; i < width; i++) yield return new Vector2I(start.x + i, start.y);
            }
        }
    }
}