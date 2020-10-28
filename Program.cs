
using System;
using System.Collections.Generic;
using System.Net.Http;


namespace Provatest_apisul
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://raw.githubusercontent.com/jeisonmarques/ProvaAdmissionalApisul/master/input.json");
                var resposta = await client.GetAsync("");
                string dados = await resposta.Content.ReadAsStringAsync();
                List<Dados> registros = new JavaScriptSerializer().Deserialize<List<Dados>>(dados);

                Dados extrairInformacao = new Dados();
                var andarMenosUtilizado = extrairInformacao.andarMenosUtilizado(registros);
                var elevadorMaisUtilizado = extrairInformacao.elevadorMaisFrequentado(registros);
                var periodoMaiorFluxo = extrairInformacao.periodoMaiorFluxoElevadorMaisFrequentado(registros);
                var elevadorMenosUtilizado = extrairInformacao.elevadorMenosFrequentado(registros);
                var periodoMenorFluxo = extrairInformacao.periodoMenorFluxoElevadorMenosFrequentado(registros);
                var periodoMaiorUtilizacao = extrairInformacao.periodoMaiorUtilizacaoConjuntoElevadores(registros);
                var percentualA = extrairInformacao.percentualDeUsoElevadorA(registros);
                var percentualB = extrairInformacao.percentualDeUsoElevadorB(registros);
                var percentualC = extrairInformacao.percentualDeUsoElevadorC(registros);
                var percentualD = extrairInformacao.percentualDeUsoElevadorD(registros);
                var percentualE = extrairInformacao.percentualDeUsoElevadorE(registros);

                Console.Write("A) O andar menos Utizado é: ");
                foreach( var item in andarMenosUtilizado)
                {
                    Console.Write(item + " ");
                }
                Console.Write("\r\nB) O elevador mais frequentado e o período de maior fluxo: ");
                foreach (var item in elevadorMaisUtilizado)
                {
                    Console.Write(item + " ");
                }
                foreach (var item in periodoMaiorFluxo)
                {
                    Console.Write(item + " ");
                }
                Console.Write("\r\nC) O elevador menos frequentado e o periodo de menor fluxo: ");
                foreach (var item in elevadorMenosUtilizado)
                {
                    Console.Write(item + " ");
                }
                foreach (var item in periodoMenorFluxo)
                {
                    Console.Write(item + " ");
                }
                Console.Write("\r\nD) O período com maior utilização dos elevadores é: ");
                foreach (var item in periodoMaiorUtilizacao)
                {
                    Console.Write(item + " ");
                }
                Console.Write("\r\nE) Percentual de utilização dos elevadores A, B, C, D e E respectivamente: ");
                Console.WriteLine(percentualA.ToString() + " " + percentualB.ToString() + " " + percentualC.ToString()
                    + " " + percentualD.ToString() + " " + percentualE.ToString());

            }
        }

        class Dados : IElevadorService
        {
            public int andar { get; set; }
            public string elevador { get; set; }
            public string turno { get; set; }

            public List<int> andarMenosUtilizado(List<Dados> lista)
            {
                List<int> andar = new List<int>();
                int[] andares = new int[16];

                // Verifica cada item da lista, e soma ao Array mais 1 ao andar correspondente
                foreach (var item in lista)
                {
                    Dados registro = item;
                    andares[registro.andar] = andares[registro.andar] + 1;
                }

                // Encontra o andar com menos ocorrências
                int menorOcorrencia = andares[0];
                for (int i = 0; i < andares.Length; i++)
                {
                    if (andares[i] < menorOcorrencia)
                    {
                        menorOcorrencia = andares[i];
                    }
                }

                // Adiciona a List<int> os andares com menor ocorrência nos dados
                for (int i = 0; i < andares.Length; i++)
                {
                    if (andares[i] == menorOcorrencia)
                    {
                        andar.Add(i);
                    }
                }

                return andar;
            }

            public List<char> elevadorMaisFrequentado(List<Dados> lista)
            {
                List<char> retorno = new List<char>();
                char[] elevadores = { 'A', 'B', 'C', 'D', 'E' };
                string stringElevadores = new string(elevadores);
                int[] elevador = new int[5];

                // verifica cada item e registra cada ocorrencia no elevador correspondente
                foreach (var item in lista)
                {
                    Dados registro = item;
                    int posicao = stringElevadores.IndexOf(registro.elevador);
                    elevador[posicao] = elevador[posicao] + 1;
                }

                // encontra a maior ocorrência
                int maior = 0;
                for (var i = 0; i < elevador.Length; i++)
                {
                    if (elevador[i] > maior)
                    {
                        maior = elevador[i];
                    }
                }

                // Adiciona a List<char> os elevadore com maior ocorrência.
                for (var i = 0; i < elevador.Length; i++)
                {
                    if (elevador[i] == maior)
                    {
                        retorno.Add(elevadores[i]);
                    }
                }

                return retorno;
            }

            public List<char> elevadorMenosFrequentado(List<Dados> lista)
            {
                List<char> retorno = new List<char>();
                char[] elevadores = { 'A', 'B', 'C', 'D', 'E' };
                string stringElevadores = new string(elevadores);
                int[] elevador = new int[5];

                // verifica cada item e registra cada ocorrencia no elevador correspondente
                foreach (var item in lista)
                {
                    Dados registro = item;
                    int posicao = stringElevadores.IndexOf(registro.elevador);
                    elevador[posicao] = elevador[posicao] + 1;
                }

                // encontra a menor ocorrência
                int menor = elevador[0];
                for (var i = 0; i < elevador.Length; i++)
                {
                    if (elevador[i] < menor)
                    {
                        menor = elevador[i];
                    }
                }

                // Adiciona a List<char> os elevadore com menor ocorrência.
                for (var i = 0; i < elevador.Length; i++)
                {
                    if (elevador[i] == menor)
                    {
                        retorno.Add(elevadores[i]);
                    }
                }

                return retorno;
            }

            public float percentualDeUso(List<Dados> lista, char elevador)
            {
                float totalDeRegistros = lista.Count;
                float registroElevador = 0;
                float percentual = 0;

                // Verifica se o item corresponde ao elevador e acrescenta +1
                foreach (var item in lista)
                {
                    Dados registro = item;
                    if (item.elevador == elevador.ToString())
                    {
                        registroElevador++;
                    }
                }
                percentual = registroElevador / totalDeRegistros * 100;
                percentual = float.Parse(percentual.ToString("N2"));
                return percentual;
            }
            public float percentualDeUsoElevadorA(List<Dados> lista)
            {
                float percentual = percentualDeUso(lista, 'A');
                return percentual;
            }

            public float percentualDeUsoElevadorB(List<Dados> lista)
            {
                float percentual = percentualDeUso(lista, 'B');
                return percentual;
            }

            public float percentualDeUsoElevadorC(List<Dados> lista)
            {
                float percentual = percentualDeUso(lista, 'C');
                return percentual;
            }

            public float percentualDeUsoElevadorD(List<Dados> lista)
            {
                float percentual = percentualDeUso(lista, 'D');
                return percentual;
            }

            public float percentualDeUsoElevadorE(List<Dados> lista)
            {
                float percentual = percentualDeUso(lista, 'E');
                return percentual;
            }

            public List<char> periodoMaiorFluxoElevadorMaisFrequentado(List<Dados> lista)
            {
                var elevadoresMaiorFluxo = elevadorMaisFrequentado(lista);
                List<char> retorno = new List<char>();
                char[] charTurnos = { 'M', 'V', 'N' };
                string stringTurnos = new string(charTurnos);

                foreach (var elevador in elevadoresMaiorFluxo)
                {

                    // verifica cada item comparando o elevador e registra a ocorrencia de cada turno
                    int[] turno = new int[3];
                    foreach (var item in lista)
                    {
                        Dados registro = item;
                        if (registro.elevador == elevador.ToString())
                        {
                            int posicao = stringTurnos.IndexOf(registro.turno);
                            turno[posicao] = turno[posicao] + 1;
                        }
                    }

                    // encontra a maior ocorrência
                    int maior = turno[0];
                    for (var i = 0; i < turno.Length; i++)
                    {
                        if (turno[i] > maior)
                        {
                            turno[i] = turno[i];
                        }
                    }

                    // Adiciona a List<char> os turnos com maior ocorrência.
                    for (var i = 0; i < turno.Length; i++)
                    {
                        if (turno[i] == maior)
                        {
                            retorno.Add(charTurnos[i]);
                        }
                    }

                }
                return retorno;

            }

            public List<char> periodoMaiorUtilizacaoConjuntoElevadores(List<Dados> lista)
            {
                List<char> retorno = new List<char>();
                char[] charTurnos = { 'M', 'V', 'N' };
                string stringTurnos = new string(charTurnos);
                int[] turno = new int[3];

                // verifica cada registro e acrescenta 1 no turno correspondente
                foreach (var item in lista)
                {
                    Dados registro = item;
                    int posicao = stringTurnos.IndexOf(registro.turno);
                    turno[posicao] = turno[posicao] + 1;

                }

                // encontra a maior ocorrência
                int maior = turno[0];
                for (var i = 0; i < turno.Length; i++)
                {
                    if (turno[i] > maior)
                    {
                        turno[i] = turno[i];
                    }
                }

                // Adiciona a List<char> os turnos com maior ocorrência.
                for (var i = 0; i < turno.Length; i++)
                {
                    if (turno[i] == maior)
                    {
                        retorno.Add(charTurnos[i]);
                    }
                }

                return retorno;
            }

            public List<char> periodoMenorFluxoElevadorMenosFrequentado(List<Dados> lista)
            {
                var elevadoresMenorFluxo = elevadorMenosFrequentado(lista);
                List<char> retorno = new List<char>();
                char[] charTurnos = { 'M', 'V', 'N' };
                string stringTurnos = new string(charTurnos);

                foreach (var elevador in elevadoresMenorFluxo)
                {

                    // verifica cada item comparando o elevador e registra a ocorrencia de cada turno
                    int[] turno = new int[3];
                    foreach (var item in lista)
                    {
                        Dados registro = item;
                        if (registro.elevador == elevador.ToString())
                        {
                            int posicao = stringTurnos.IndexOf(registro.turno);
                            turno[posicao] = turno[posicao] + 1;
                        }
                    }

                    // encontra a menor ocorrência
                    int menor = turno[0];
                    for (var i = 0; i < turno.Length; i++)
                    {
                        if (turno[i] < menor)
                        {
                            turno[i] = turno[i];
                        }
                    }

                    // Adiciona a List<char> os turnos com menor ocorrência.
                    for (var i = 0; i < turno.Length; i++)
                    {
                        if (turno[i] == menor)
                        {
                            retorno.Add(charTurnos[i]);
                        }
                    }

                }
                return retorno;
            }
        }


        interface IElevadorService
        {
            /// <summary> Deve retornar uma List contendo o(s) andar(es) menos utilizado(s). </summary> 
            List<int> andarMenosUtilizado(List<Dados> lista);

            /// <summary> Deve retornar uma List contendo o(s) elevador(es) mais frequentado(s). </summary> 
            List<char> elevadorMaisFrequentado(List<Dados> lista);

            /// <summary> Deve retornar uma List contendo o período de maior fluxo de cada um dos elevadores mais frequentados (se houver mais de um). </summary> 
            List<char> periodoMaiorFluxoElevadorMaisFrequentado(List<Dados> lista);

            /// <summary> Deve retornar uma List contendo o(s) elevador(es) menos frequentado(s). </summary> 
            List<char> elevadorMenosFrequentado(List<Dados> lista);

            /// <summary> Deve retornar uma List contendo o período de menor fluxo de cada um dos elevadores menos frequentados (se houver mais de um). </summary> 
            List<char> periodoMenorFluxoElevadorMenosFrequentado(List<Dados> lista);

            /// <summary> Deve retornar uma List contendo o(s) periodo(s) de maior utilização do conjunto de elevadores. </summary> 
            List<char> periodoMaiorUtilizacaoConjuntoElevadores(List<Dados> lista);

            /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador A em relação a todos os serviços prestados. </summary> 
            float percentualDeUsoElevadorA(List<Dados> lista);

            /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador B em relação a todos os serviços prestados. </summary> 
            float percentualDeUsoElevadorB(List<Dados> lista);

            /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador C em relação a todos os serviços prestados. </summary> 
            float percentualDeUsoElevadorC(List<Dados> lista);

            /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador D em relação a todos os serviços prestados. </summary> 
            float percentualDeUsoElevadorD(List<Dados> lista);

            /// <summary> Deve retornar um float (duas casas decimais) contendo o percentual de uso do elevador E em relação a todos os serviços prestados. </summary> 
            float percentualDeUsoElevadorE(List<Dados> lista);
        }
    }

    internal class JavaScriptSerializer
    {
        public JavaScriptSerializer()
        {
        }

        internal T Deserialize<T>(string dados)
        {
            throw new NotImplementedException();
        }
    }
}