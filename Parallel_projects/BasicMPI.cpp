// HelloWorld.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "mpi.h"
#include "stdio.h"
#include "stdlib.h"

int main(int argc, char* argv[])
{
	MPI_Init(&argc, &argv);

	int rank;

	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	if (rank == 0)
	{
		char helloStr[] = "Hello World";
		//MPI_Send(helloStr, _countof(helloStr), MPI_CHAR, 2, 0, MPI_COMM_WORLD);
		MPI_Send(helloStr, _countof(helloStr), MPI_CHAR, 3, 0, MPI_COMM_WORLD);
	}
	else if (rank == 1)
	{
		char helloStr[] = "Hello World";
		//MPI_Recv(helloStr, _countof(helloStr), MPI_CHAR, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		//printf("Rank 1 recieved string %s from Rank 0 \n", helloStr);
		MPI_Send(helloStr, _countof(helloStr), MPI_CHAR, 2, 0, MPI_COMM_WORLD);
		printf("Rank 1 recieved nothing");
	}
	else if (rank == 2)
	{
		char helloStr[12];
		//MPI_Recv(helloStr, _countof(helloStr), MPI_CHAR, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		//printf("Rank 2 recieved string %s from Rank 0 \n", helloStr);
		MPI_Recv(helloStr, _countof(helloStr), MPI_CHAR, 1, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		printf("Rank 2 recieved string %s from Rank 1 \n", helloStr);
		//printf("Rank 2 recieved nothing");
	}
	else if (rank == 3)
	{
		char helloStr[12];
		MPI_Recv(helloStr, _countof(helloStr), MPI_CHAR, 0, 0, MPI_COMM_WORLD, MPI_STATUS_IGNORE);
		printf("Rank 3 recieved string %s from Rank 0 \n", helloStr);
	}
	MPI_Finalize();

	return 0;
}
