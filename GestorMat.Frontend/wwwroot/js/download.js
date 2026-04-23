export function downloadFileFromStream(fileName, contentStream) {
    const blob = new Blob([contentStream]);
    const url = URL.createObjectURL(blob);

    const anchor = document.createElement('a');
    anchor.href = url;
    anchor.download = fileName ?? '';
    anchor.click();

    anchor.remove();
    URL.revokeObjectURL(url);
}

window.downloadFile = (fileName, byteArray) => {
    const blob = new Blob([new Uint8Array(byteArray)], { type: "application/pdf" });

    const url = URL.createObjectURL(blob);

    const link = document.createElement('a');
    link.href = url;
    link.download = fileName ?? 'archivo.pdf';

    document.body.appendChild(link);
    link.click();

    document.body.removeChild(link);
    URL.revokeObjectURL(url);

};