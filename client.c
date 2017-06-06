#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <sys/socket.h>
#include <netinet/in.h>
#include <arpa/inet.h>
#include <netdb.h>
#include <unistd.h>
#include <errno.h>

#define BUFLEN 1024

/* requires 3 parameters in order: server address, port, file */
int main( int argc, char *argv[] )
{
    int sd, rc, length, port, readsize = sizeof(int);
    struct sockaddr_in serveraddr;
    char buffer[BUFLEN];
    char server[255];
    char temp;
    char c;
    int cnt = 0;
    struct hostent *hostp;

    FILE *fp;


    /*handle args*/
    if( argc = 4 && (port = atoi(argv[2])) != 0 )
    {
        if( (fp = fopen( argv[3], "rb" )) != NULL )
        {
            strcpy(server, argv[1]);
        
            printf( "Connecting to %s, port %d ...\n", server, port );
        }
        else
        {
            printf( "file read error" );
            exit(-1);
        }
    }
    else
    {
        printf( "usage: %s <server address> <port> <file>\n", argv[0] );
        exit(-1);
    }
    
    /* The socket() function returns a socket */
    /* descriptor representing an endpoint. */
    /* The statement also identifies that the */
    /* INET (Internet Protocol) address family */
    /* with the TCP transport (SOCK_STREAM) */
    /* will be used for this socket. */
    /******************************************data/
	/* get a socket descriptor */
    if( ( sd = socket( AF_INET, SOCK_STREAM, 0 ) ) < 0 )
    {
        perror( "Client-socket() error\n" );
        exit(-1);
    }
    else
    {
        printf( "Client-socket() OK\n" );
    }


    memset( &serveraddr, 0x00, sizeof( struct sockaddr_in ) );
    serveraddr.sin_family = AF_INET;
    serveraddr.sin_port = htons( port );

    if( ( serveraddr.sin_addr.s_addr = inet_addr( server ) ) == (unsigned long)INADDR_NONE )
    {
        /* get host address */
        hostp = gethostbyname( server );
        if( hostp == ( struct hostent * )NULL )
        {
            printf( "HOST NOT FOUND --> " );
            /* h_errno is usually defined */
            /* in netdb.h */
            printf( "h_errno = %d\n", h_errno );
            printf( "---This is a client program---\n" );
            printf( "Command usage: %s <server name or IP>\n", argv[0] );
            close( sd );
            exit( -1 );
        }
        memcpy( &serveraddr.sin_addr, hostp->h_addr, sizeof( serveraddr.sin_addr ) );
    }

    /* After the socket descriptor is received, the */
    /* connect() function is used to establish a */
    /* connection to the server. */
    /***********************************************/
    /* connect() to server. */
    if( ( rc = connect( sd, ( struct sockaddr * )&serveraddr, sizeof( serveraddr ) ) ) < 0 )
    {
        perror( "Client-connect() error" );
        close( sd );
        exit( -1 );
    }
    else
        printf( "Connection established...\n" );

    /*send file to the server */
    while (!feof(fp))
    {
        readsize = fread( buffer, 1, BUFLEN, fp );
        printf("readsize: %d", readsize);
        rc = write( sd, buffer, readsize );

        if(rc < 0)
        {
            perror( "Client-write() error" );
            rc = getsockopt( sd, SOL_SOCKET, SO_ERROR, &temp, &length );
            if( rc == 0 )
            {
                /* Print out the asynchronously received error. */
                errno = temp;
                perror( "SO_ERROR was" );
            }
            close( sd );
            exit( -1 );
        }
        else
        {
            printf( "\nwrite %d OK\n", cnt );
        }
        cnt++;
    }
    
    /* Close socket descriptor from client side. */
    close( sd );
    exit( 0 );
    return 0;
}
