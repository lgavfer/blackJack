using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


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

    // Utilizamos esta lista en el metodo ShuffleCards() para almacenar el orden de las cartas
    public List<int> cardOrders = new List<int>();
       
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

    // Metodo para barajar las cartas automaticamente
    private void ShuffleCards()
    {
        /*TODO:
         * Barajar las cartas aleatoriamente.
         * El metodo Random.Range(0,n), devuelve un valor entre 0 y n-1
         * Si lo necesitas, puedes definir nuevos arrays.
         */   

        // El bucle se ejecuta 52 veces -> tenemos 52 cartas
         for (int i = 0; i < 52; i++)
        {
            // Genera un número aleatorio entre el 0 y el 51 (tantos índices como cartas)
            int randomNumber = Random.Range(0, 52);
            // El ciclo "while" comprueba que el número no se ha seleccionado ya
            while (cardOrders.Contains(randomNumber))
            {
                // En el caso de que se haya seleccionado -> sigue buscando hasta encontrar un índice que no se esté usando
                randomNumber = Random.Range(0, 52);
            }
            cardOrders.Add(randomNumber);
        }    
    }

    void StartGame()
    {

        // Creamos una varible que utilizaremos mas adelante para comprobar si se ha finalizado el juego
        bool gameEnded = false;

        // Al principio se reparten dos cartas
        for (int i = 0; i < 2; i++)
        {
            PushPlayer();
            PushDealer();
            
            // Comprobamos si alguno de los dos tiene Blackjack
            if(player.GetComponent<CardHand>().points == 21 || dealer.GetComponent<CardHand>().points == 21) {
                // Desactivamos los botones del juego
                hitButton.interactable = false;
                stickButton.interactable = false;
                // Mostramos el mensaje que indica el final del juego
                finalMessage.text = "Fin del juego. ¡Se ha obtenido BlackJack";
                // Damos el juego por terminado
                gameEnded = true;
            }
        }
        // En caso de que no se haya obtenido BlackJack y el juego no haya terminado actualizamos los marcadores que indican la puntuación
        if(!gameEnded) {
            // Actualizamos en pantalla las puntuaciones de las cartas
            Puntosplayer.text = player.GetComponent<CardHand>.points.ToString();
            Puntosdealer.text = dealer.GetComponent<CardHand>.points.ToString();
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

        // Declaramos e inicializamos variables
        int casosFavorables = 0; // Variable para almacenar el número de casos favorables encontrados
        int[] cartasMesa = new int[3]; // Array para almacenar las cartas de la mesa (2 del jugador y 1 del dealer)


        // Primero calculamos la probabilidad de que el Dealer tenga más puntuación que el jugador 

        // Obtenemos la puntuación del jugador y la carta oculta del dealer
        int puntuacionJugador = player.GetComponent<CardHand>().points;
        int cartaDealer = dealer.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;

        // Calculamos la diferencia entre la puntuación del jugador y la carta oculta del dealer
        int diferencia = puntuacionJugador - cartaDealer;

        // Almacenamos las cartas del jugador y la carta oculta del dealer en el array
        cartasMesa[0] = player.GetComponent<CardHand>().cards[0].GetComponent<CardModel>().value;
        cartasMesa[1] = player.GetComponent<CardHand>().cards[1].GetComponent<CardModel>().value;
        cartasMesa[2] = cartaDealer;

        // Si la diferencia es menor que cero, la probabilidad de que el dealer tenga más puntuación es cero
        if (diferencia < 0)
        {
            probMessage.text = 0.ToString();
            return 0;
        }

        

        // Recorremos los valores posibles de puntuación del dealer, desde la diferencia hasta el 11 (valor máximo posible)
        for (int i = diferencia + 1; i < 12; i++)
        {
            int contadorCartas = 0;

            // Contamos las cartas en la mesa que tienen el mismo valor que la puntuación actual del dealer
            if (i == cartasMesa[0])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[1])
            {
                contadorCartas++;
            }
            if (i == cartasMesa[2])
            {
                contadorCartas++;
            }

            // Calculamos el número de casos favorables, dependiendo del valor actual del dealer
            if (i != 10)
                casosFavorables = casosFavorables + (4 - contadorCartas); // Cualquier carta que no sea un 10 tiene 4 naipes
            if (i == 10)
            {
                casosFavorables = casosFavorables + (16 - contadorCartas); // Las cartas con valor de 10 (J,Q,K) tienen 16 naipes
            }
        }

        // Mostramos en consola el número de casos favorables
        Debug.Log("Casos favorables Mas que dealer " + casosFavorables);

        


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
