// Assignment3_MPI.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "mpi.h"
#include "stdio.h"
#include "stdlib.h"
#include "time.h"

#define SIZE 20000

int compare(const void *a, const void * b)
{
	return(*(int*)a - *(int*)b);
}

int main(int argc, char* argv[])
{
	MPI_Init(&argc, &argv);
	
	int rank;
	clock_t t;
	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	//counter variables for later
	int i = 0;
	int j = 0;
	int k = 0;
	int l = 0;

	if(rank == 0)
	{
		int arr[SIZE];
		int solved[SIZE];
		int first_half[SIZE / 2];
		int second_half[SIZE / 2];
		//these 4 are used for a sort that uses 4 different ranks
		int quad1[SIZE / 4];
		int quad2[SIZE / 4];
		int quad3[SIZE / 4];
		int quad4[SIZE / 4];
		//fill arrays with random values from 0 to 99

		for (int i = 0; i < SIZE; i++)
		{
			arr[i] = rand() % 100;
			if (i < (SIZE / 2))
			{
				first_half[i] = arr[i];
			}
			else
			{
				second_half[i % (SIZE / 2)] = arr[i];
			}
			//fill 4 arrays to be sorted in 4 different ranks
			
			if (i < (SIZE / 4))
			{
				quad1[i] = arr[i];
			}
			else if (i < (SIZE / 2))
			{
				quad2[i % (SIZE / 4)] = arr[i];
			}
			else if (i < (SIZE - (SIZE / 4)))
			{
				quad3[i % (SIZE / 4)] = arr[i];
			}
			else
			{
				quad4[i % (SIZE / 4)] = arr[i];
			}
		}
		
		//calculate serially
		t = clock();
		qsort(arr, _countof(arr), sizeof(int), compare);
		t = clock() - t;
		printf("Run time using a serial implementation: %.3f\n", ((float)t) / CLOCKS_PER_SEC);
		//calculate using 2 processes
		t = clock();
		MPI_Send(second_half, _countof(second_half), MPI_INT, 1, 0, MPI_COMM_WORLD);

		qsort(first_half, SIZE / 2, sizeof(int), compare);
		
		//recieve sorted second half of array, then merge the two sorted halves into arr
		MPI_Recv(second_half, _countof(second_half), MPI_INT, 1, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);

		i = 0;
		j = 0;
		while ((i + j) < SIZE)
		{
			if ((first_half[i] <= second_half[j]) || (j >= (SIZE / 2)))
			{
				solved[i + j] = first_half[i];
				i++;
				//printf("\nI = %d", i);
			}
			else
			{
				solved[i + j] = second_half[j];
				j++;
				//printf("\nj = %d", j);
			}
		}
		t = clock() - t;
		printf("Processing time using %d elements and 2 processes: %.3f seconds.\n", SIZE,  ((float)t) / CLOCKS_PER_SEC);
		
		//now do the same with 4 processes
		t = clock();

		MPI_Send(quad2, _countof(quad2), MPI_INT, 2, 0, MPI_COMM_WORLD);
		MPI_Send(quad3, _countof(quad3), MPI_INT, 3, 0, MPI_COMM_WORLD);
		MPI_Send(quad4, _countof(quad4), MPI_INT, 4, 0, MPI_COMM_WORLD);

		qsort(quad1, SIZE / 4, sizeof(int), compare);

		MPI_Recv(quad2, _countof(quad2), MPI_INT, 2, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		MPI_Recv(quad3, _countof(quad3), MPI_INT, 3, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		MPI_Recv(quad4, _countof(quad4), MPI_INT, 4, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);


		i = 0;
		j = 0;
		//these values are used to tell if the subarray has been finished merging into the bigger one
		bool i_check = false;
		bool j_check = false;
		bool k_check = false;
		bool l_check = false;

		while ((i + j + k + l) < SIZE)
		{
			//if a subarray finished merging it would increment out of bounds and throw off the evaluation, hence the following decrement and using the boolean values
			if (i == (SIZE / 4))
			{
				i--;
				i_check = true;
			}
			if (j == (SIZE / 4))
			{
				j--;
				j_check = true;
			}
			if (k == (SIZE / 4))
			{
				k--;
				k_check = true;
			}
			if (l == (SIZE / 4))
			{
				l--;
				l_check = true;
			}

			if ((((quad1[i] <= quad2[j]) || (j_check == true)) && ((quad1[i] <= quad3[k]) || (k_check == true)) && ((quad1[i] <= quad4[l]) || (l_check == true))) && (i_check == false))
			{
				solved[i + j + k + l] = quad1[i];
				i++;
				//printf("\nI = %d", i);
			}
			else if((((quad2[j] <= quad3[k]) || (k_check == true)) && ((quad2[j] <= quad4[l]) || (l_check == true))) && (j_check == false))
			{
				solved[i + j + k + l] = quad2[j];
				j++;
				//printf("\nj = %d", j);
			}
			else if (((quad3[k] <= quad4[l]) || (l_check == true)) && (k_check == false))
			{
				solved[i + j + k + l] = quad3[k];
				k++;
				//printf("\nk = %d", k);
			}
			else
			{
				solved[i + j + k + l] = quad4[l];
				
				l++;
				//printf("\nl = %d", l);
			}
			//printf("\nI = %d", i);
			//printf("\nj = %d", j);
			//printf("\nk = %d", k);
			//increment all back to size / 4
			if (i_check && j_check && k_check && l_check)
			{
				i++;
				j++;
				k++;
			}

		}
		t = clock() - t;
		printf("Processing time using %d elements and 4 processes: %.3f seconds.", SIZE, ((float)t) / CLOCKS_PER_SEC);
	}
	if (rank == 1)
	{
		int arr[SIZE / 2];
		MPI_Recv(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		qsort(arr, SIZE / 2, sizeof(int), compare);
		//send sorted array back to rank 0
		MPI_Send(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD);
	}
	if (rank == 2)
	{
		int arr[SIZE / 4];
		MPI_Recv(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		qsort(arr, SIZE / 4, sizeof(int), compare);
		//send sorted array back to rank 0
		MPI_Send(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD);
	}
	if (rank == 3)
	{
		int arr[SIZE / 4];
		MPI_Recv(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		qsort(arr, SIZE / 4, sizeof(int), compare);
		//send sorted array back to rank 0
		MPI_Send(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD);
	}
	if (rank == 4)
	{
		int arr[SIZE / 4];
		MPI_Recv(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		qsort(arr, SIZE / 4, sizeof(int), compare);
		//send sorted array back to rank 0
		MPI_Send(arr, _countof(arr), MPI_INT, 0, 0, MPI_COMM_WORLD);
	}


	MPI_Finalize();
    return 0;
}



