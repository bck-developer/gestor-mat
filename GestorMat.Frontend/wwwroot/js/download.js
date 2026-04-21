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