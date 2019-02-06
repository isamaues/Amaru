#import <stdio.h>
#import <stdlib.h>

int main() {
	char nome[30];
	float nota[3];
	float media;
	
	printf("Insira o nome do aluno");
	scanf("%s",nome);
	
	for (int i = 0; i < 3; i++){
		printf("Insira  a nota %d",i);
		scanf("%f",&nota[i];
	}
	
	media = (nota[0]+nota[1]+nota[2])/3;
	
	if (media >= 7) {
			printf("O aluno %s foi aprovado com a media %f",nome,media);
	}
	else {
			printf("O aluno %s nao foi aprovado com a media %f",nome,media);
	}
	
}

#import <stdio.h>
#import <stdlib.h>

int main() {
	int idade;
	
	for (int i = 0; i < 3; i++){
	   printf("Insira a idade da pessoa");
	   scanf("%d",&idade);
	   if (idade >= 18) {
	      printf("A pessoa e maior de idade");
	   }
	   else {
	      printf("A pessoa e menor de idade");
	  }
	}
}

#import <stdio.h>
#import <stdlib.h>

int main() {
	int maior = 0, menor = 0 ;
	int par = 0,imp = 0,num;
	
	for (int i = 0; i < 10; i++){
	   printf("Insira um numero");
	   scanf("%d",&num);
	   if (num % 2 == 0) {
	      par++;
	   }
	   else {
	      impar++;
	  }
	  if (num > maior) {
	     maior = num;
	  }
	   else if (num < menor || menor == 0) {
	      menor = num;
	   }
	}
	
	printf("Pares:%d Impares:%d \n Maior:%d Menor:%d");
}


#import <stdio.h>
#import <stdlib.h>

int main() {
	int A[10][10], B[10][10], P[10][10] ;
	int m,n,p;
	
	printf("Insira o valor de m(maximo 10)");
	scanf("%d",&m);
	printf("Insira o valor de n(maximo 10)");
	scanf("%d",&n);
	printf("Insira o valor de p(maximo 10)");
	scanf("%d",&p);
	
	for (int i = 0, i < m, i++) {
	     for (int j = 0; j < n, j++) {
	         printf("Insira A[%d][%d]:",i,j);
	         scanf("%d",&A[i][j]);
	     }
	}
	
	for (int i = 0, i < n, i++) {
	     for (int j = 0; j < p, j++) {
	         printf("Insira B[%d][%d]:",i,j);
	         scanf("%d",&B[i][j]);
	     }
	}
	
	for (int i = 0, i < n, i++) {
	     for (int j = 0; j < n, ij++) {
	         P[i][j] = 0;
	         for (int k = 0; k < n, k++) {
		    P[i][j] += A[k][i] * B[j][k];
	         }
	     }
	}
	
	for (int i = 0, i < n, i++) {
	     for (int j = 0; j < p, j++) {
	         printf("%d ", P[i][j]);
	     }
	     printf("\n");
	}
	system("pause");
}

#import <stdio.h>
#import <stdlib.h>

int main() {
	int n;
	int exp = 1;
	float fra = 0;
	
	printf("Insiara o valor de N");
	scanf("%d",&n);
	
	for (int i = 1; i <= n; if++) {
	    fra += 1/i^exp;
	    
	    if (exp % 3 == 0 && exp > 3)
	      exp=0;
	    exp ++;
	}
}

#import <stdio.h>
#import <stdlib.h>

int main() {
	int seq[100];
	int cont;
	
	for (int i = 2; i < 100; i++){
		if(seq[i-2] == seq[i-1] && seq[i-1] == seq[i]) {
                     cont++;
		}	
	}
	
	media = (nota[0]+nota[1]+nota[2])/3;
	
	if (media >= 7) {
			printf("O aluno %s foi aprovado com a media %f",nome,media);
	}
	else {
			printf("O aluno %s nao foi aprovado com a media %f",nome,media);
	}
	
}
