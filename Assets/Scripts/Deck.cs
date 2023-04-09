using UnityEngine;
using UnityEngine.UI;

public class Deck : MonoBehaviour
{
    public Sprite[] faces;
    public GameObject dealer;
    public GameObject player;
    public Button hitButton;
    public Button stickButton;
    public Button playAgainButton;
    public Text finalMessage;
    public Text probMessage;

    public int[] values = new int[52];
    int cardIndex = 0;    
       
    private void Awake()
    {    
        InitCardValues();        

    }

    private void Start()
    {
        ShuffleCards();
        StartGame();        
    }

    private void InitCardValues()
    {
        /*TODO:
         * Asignar un valor a cada una de las 52 cartas del atributo "values".
         * En principio, la posicion de cada valor se debera corresponder con la posicion de faces. 
         * Por ejemplo, si en faces[1] hay un 2 de corazones, en values[1] deberia haber un 2.
         */

        
        // El bucle recorre cada una de las cartas de la lista values[] que hemos inicializado antes
        for(int i = 0; i < values.Length; i++)
        {
            // Le da el valor 11 -> (A) -> a la primera carta
            if (cardIndex==0)
            {
                values[i] = 11;
                // Sumamos una unidad al indice de las cartas
                cardIndex += 1;
            }
            // A las siguientes 9 cartas -> de la 2 a la 10 -> le asinamos el valor que le corresponde
            // Al dos de corazones -> valor 2
            // Al tres de corazones -> valor 3...
            else if (cardIndex <10)
            {
                values[i] = cardIndex + 1;
                // Vamos sumando una unidad al indice para ir guardando el valor
                cardIndex += 1;
            }
            // Una vez hemos dado valor a las 10 primeras cartas incluyendo las (A) -> le damos valor a J, Q, K
            else if(cardIndex<13)
            {
                // A las cartas J, Q, K -> le añadimos el valor 10
                values[i] = 10;
                // El siguiente if comprueba si hemos completado toda la lista de carta del mismo palo
                // Si ya le hemos dado valor a J, K y Q
                if (cardIndex >= 12) // Es >= 12 porque las posiciones son siempre una menos del numero -> la lista empieza en [0], no es 1
                {
                    // el indice de la carta vuelve a ser 0 para pasar a dar valores al siguiente palo
                    cardIndex = 0;
                }
                else
                {
                    // Si todavia no le hemos dado valor a las 13 cartas del palo, seguimos sumando una unidad al indice hasta hacerlo
                    cardIndex += 1;
                }
            }
        }
        // El bucle termina cuando le hemos dado valor a las 52 cartas
    }

    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El método Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */       
    }

    void StartGame()
    {
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            /*TODO:
             * Si alguno de los dos obtiene Blackjack, termina el juego y mostramos mensaje
             */
        }
    }

    private void CalculateProbabilities()
    {
        /*TODO:
         * Calcular las probabilidades de:
         * - Teniendo la carta oculta, probabilidad de que el dealer tenga más puntuación que el jugador
         * - Probabilidad de que el jugador obtenga entre un 17 y un 21 si pide una carta
         * - Probabilidad de que el jugador obtenga más de 21 si pide una carta          
         */
    }

    void PushDealer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        dealer.GetComponent<CardHand>().Push(faces[cardIndex],values[cardIndex]);
        cardIndex++;        
    }

    void PushPlayer()
    {
        /*TODO:
         * Dependiendo de cómo se implemente ShuffleCards, es posible que haya que cambiar el índice.
         */
        player.GetComponent<CardHand>().Push(faces[cardIndex], values[cardIndex]/*,cardCopy*/);
        cardIndex++;
        CalculateProbabilities();
    }       

    public void Hit()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */
        
        //Repartimos carta al jugador
        PushPlayer();

        /*TODO:
         * Comprobamos si el jugador ya ha perdido y mostramos mensaje
         */      

    }

    public void Stand()
    {
        /*TODO: 
         * Si estamos en la mano inicial, debemos voltear la primera carta del dealer.
         */

        /*TODO:
         * Repartimos cartas al dealer si tiene 16 puntos o menos
         * El dealer se planta al obtener 17 puntos o más
         * Mostramos el mensaje del que ha ganado
         */                
         
    }

    public void PlayAgain()
    {
        hitButton.interactable = true;
        stickButton.interactable = true;
        finalMessage.text = "";
        player.GetComponent<CardHand>().Clear();
        dealer.GetComponent<CardHand>().Clear();          
        cardIndex = 0;
        ShuffleCards();
        StartGame();
    }
    
}
