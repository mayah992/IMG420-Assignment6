using Godot;

public partial class BTSelector : BTNode
{
    private int _currentIndex = 0;

    public override BTState Tick(Enemy enemy, double delta)
    {
		_currentIndex = 0;
        while (_currentIndex < GetChildCount())
        {
            var child = GetChild<BTNode>(_currentIndex);
            var result = child.Tick(enemy, delta);

            switch (result)
            {
                case BTState.Running:
                    return BTState.Running;

                case BTState.Success:
                    _currentIndex = 0;
                    return BTState.Success;

                case BTState.Failure:
                    _currentIndex++;
                    break;
            }
        }
		
        return BTState.Failure;
    }
}
