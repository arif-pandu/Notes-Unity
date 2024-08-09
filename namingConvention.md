Unity C# naming convention

**camelCase:**

-  Used for local variables
-  Used for private fields
Example: playerHealth, enemyCount


**PascalCase:**

-  Used for public properties
-  Used for method names
Used for class names
Example: PlayerHealth, GetEnemyCount()


**_camelCase:**

-  Often used for private fields, especially when they have a corresponding public property
Example: private int _health;


**ALL_CAPS:**

-  Used for constants
Example: const int MAX_PLAYERS = 4;


Snake case (e.g., player_health) is not commonly used in C# or Unity.

Here's an example that demonstrates these conventions:
csharpCopypublic class PlayerController : MonoBehaviour
{
    public int MaxHealth { get; private set; }
    
    private int _currentHealth;
    private float movementSpeed;

    public const int PLAYER_LIVES = 3;

    public void TakeDamage(int damageAmount)
    {
        int remainingHealth = _currentHealth - damageAmount;
        SetHealth(remainingHealth);
    }

    private void SetHealth(int newHealth)
    {
        _currentHealth = Mathf.Clamp(newHealth, 0, MaxHealth);
    }
}
